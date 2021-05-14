using MediatR;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiendaServicios.RabbitMQ.Lib.BusRabbit;
using TiendaServicios.RabbitMQ.Lib.Commands;
using TiendaServicios.RabbitMQ.Lib.Events;

namespace TiendaServicios.RabbitMQ.Lib.Impl
{
    public class RabbitEventBus : IRabbitEventBus
    {
        private readonly IMediator mediator;

        private readonly Dictionary<string, List<Type>> _manejadores;

        private readonly List<Type> _eventTypes;

        public RabbitEventBus(IMediator mediator)
        {
            this.mediator = mediator;
            _manejadores = new Dictionary<string, List<Type>>();
            _eventTypes = new List<Type>();
        }

        public void Publish<T>(T @event) where T : Event
        {
            var factory = new ConnectionFactory() { HostName = "rabbit-microservice-web" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel()) {
                var eventName = @event.GetType().Name;

                channel.QueueDeclare(eventName, false, false, false, null);

                var message = JsonConvert.SerializeObject(@event);
                var body = Encoding.UTF8.GetBytes(message);
                channel.BasicPublish("", eventName, null, body);
            }
        }

        public Task SendCommand<T>(T command) where T : Command
        {
            return mediator.Send(command);
        }

        public void Subscribe<T, TH>()
            where T : Event
            where TH : IEventHandler<T>
        {
            var eventName = typeof(T).Name;
            var handlerEventType = typeof(TH);

            if (!_eventTypes.Contains(typeof(T)))
            {
                _eventTypes.Add(typeof(T));
            }

            if (!_manejadores.ContainsKey(eventName))
            {
                _manejadores.Add(eventName, new List<Type>());
            }

            if(_manejadores[eventName].Any(x => x.GetType() == handlerEventType))
            {
                throw new ArgumentException($"El manejador {handlerEventType.Name} fue registrado anteriormente por {eventName}");
            }

            _manejadores[eventName].Add(handlerEventType);

            var factory = new ConnectionFactory()
            {
                HostName = "rabbit-microservice-web",
                DispatchConsumersAsync = true
            };

            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();

            channel.QueueDeclare(eventName, false, false, false, null);

            var consumer = new AsyncEventingBasicConsumer(channel);
            consumer.Received += Consumer_Delegate;

            channel.BasicConsume(eventName, true, consumer);
        }

        private async Task Consumer_Delegate(object sender, BasicDeliverEventArgs @event)
        {
            var nombreEvento = @event.RoutingKey;
            var message = Encoding.UTF8.GetString(@event.Body.ToArray());

            try
            {
                if (_manejadores.ContainsKey(nombreEvento))
                {
                    var suscriptions = _manejadores[nombreEvento];
                    foreach(var sb in suscriptions)
                    {
                        var manejador = Activator.CreateInstance(sb);
                        if (manejador == null) continue;

                        var tipoEvento = _eventTypes.SingleOrDefault(x => x.Name == nombreEvento);
                        var eventoDS = JsonConvert.DeserializeObject(message, tipoEvento);

                        var tipoConcreto = typeof(IEventHandler<>).MakeGenericType(tipoEvento);

                        await (Task)tipoConcreto.GetMethod("Handle").Invoke(manejador, new object[] { eventoDS });
                    }
                }
            }
            catch (Exception e)
            {

            }
        }
    }
}

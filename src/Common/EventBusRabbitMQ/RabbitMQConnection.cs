using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EventBusRabbitMQ
{
    public class RabbitMQConnection : IRabbitMQConnection
    {
        private readonly IConnectionFactory _connectionFactory;
        private IConnection _connection;
        private bool _dipsosed;

        public RabbitMQConnection(IConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
            if ( !IsConnected )
            {
                TryConnect();
            }
        }
        public bool IsConnected
        {
            get
            {
                return _connection != null && _connection.IsOpen && !_dipsosed;
            }
        }

        public IModel CreateModel()
        {
            if ( !IsConnected )
            {
                throw new InvalidOperationException("No rabbit connection"); 
            }

            return _connection.CreateModel();
        }

        public void Dispose()
        {
            if ( _dipsosed )
            {
                return;
            }

            try
            {
                _connection.Dispose();
            }
            catch ( Exception )
            {

                throw;
            }
        }

        public bool TryConnect()
        {
            try
            {
                _connection = _connectionFactory.CreateConnection();
            }
            catch ( BrokerUnreachableException )
            {
                Thread.Sleep(2000);
                _connection = _connectionFactory.CreateConnection();
                
            }

            if ( IsConnected )
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}

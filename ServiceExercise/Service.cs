using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ServiceExercise
{
    public class Service : IService
    {
        private readonly int _connectionCount;//Connections sent by client
        private readonly List<Connection> _connections;//Connection list to handle the sent connections
        private readonly Queue<Request> _requestQueue;//Queue to handle the number of proccesed connections

        public Service(int connectionCount)
        {
            _connectionCount = connectionCount;
            _connections = new List<Connection>();
            _requestQueue = new Queue<Request>();

            for (int i = 0; i < _connectionCount; i++)
            {
                _connections.Add(new Connection());
            }
        }

        public void sendRequest(Request request)
        {
            //Add requests to the queue
            _requestQueue.Enqueue(request);
            processRequests(request.Command);
        }

        public int getSummary()
        {
            return _requestQueue.Count;
        }

        private void processRequests(int _value)
        {
            //int counter = 0;
            foreach (Connection conn in _connections)
            {
                //counter++;
                conn.runCommand(_value);
            }
        }
    }
}

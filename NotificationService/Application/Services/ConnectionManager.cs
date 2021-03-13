using System;
using System.Collections.Generic;
using System.Linq;

namespace NotificationService.Application.Services
{
    public class ConnectionManager : IConnectionManager
    {
        private readonly Dictionary<Guid, IList<string>> _connections = new();

        public int Count => _connections.Count;

        public void Add(Guid userId, string connectionId)
        {
            lock (_connections)
            {
                if (!_connections.TryGetValue(userId, out var connections))
                {
                    connections = new List<string>();
                    _connections.Add(userId, connections);
                }

                lock (connections)
                {
                    connections.Add(connectionId);
                }
            }
        }

        public IEnumerable<string> Get(Guid userId)
        {
            lock (_connections)
            {
                if (_connections.TryGetValue(userId, out var connections)) return connections;

                return Enumerable.Empty<string>();
            }
        }

        public void Remove(Guid userId, string connectionId)
        {
            lock (_connections)
            {
                if (!_connections.TryGetValue(userId, out var connections)) return;

                lock (connections)
                {
                    connections.Remove(connectionId);

                    if (connections.Count == 0) _connections.Remove(userId);
                }
            }
        }
    }
}

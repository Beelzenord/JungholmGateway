using System;
using Supabase;

namespace JungholmInstruments.Services
{
    public class SupabaseService
    {
        private static SupabaseService? _instance;
        private Client? _client;

        private SupabaseService()
        {
        }

        public static SupabaseService Instance => _instance ??= new SupabaseService();

        public void Initialize(string url, string anonKey)
        {
            var options = new SupabaseOptions
            {
                AutoConnectRealtime = false
            };

            _client = new Client(url, anonKey, options);
        }

        public Client Client
        {
            get
            {
                if (_client == null)
                    throw new InvalidOperationException("Supabase client not initialized. Call Initialize() first.");
                return _client;
            }
        }

        public bool IsInitialized => _client != null;
    }
}


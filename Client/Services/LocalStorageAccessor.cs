using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace Aon_PMS.Services
{
    public class LocalStorageAccessor : IAsyncDisposable
    {
        private Lazy<IJSObjectReference> _accessorJsRef = new();
        private readonly IJSRuntime _jsruntime;

        public LocalStorageAccessor(IJSRuntime jSRuntime)
        {
            _jsruntime = jSRuntime;
        }

        private async Task WaitForReference()
        {
            if(_accessorJsRef.IsValueCreated is false)
            {
                _accessorJsRef = new(await _jsruntime.InvokeAsync<IJSObjectReference>("import", "./Js/LocalStorageAccessor.js"));
            }
        }

        public async ValueTask DisposeAsync()
        {
            if (_accessorJsRef.IsValueCreated)
            {
                await _accessorJsRef.Value.DisposeAsync();
            }
        }

        public async Task<string> GetValueAsync(string key)
        {
            await WaitForReference();
            var result = await _accessorJsRef.Value.InvokeAsync<string>("get", key);

            return result;
        }

        public async Task SteValueAsync<T>(string Key, T value)
        {
            await WaitForReference();
            var data = JsonSerializer.Serialize(value);
            await _accessorJsRef.Value.InvokeVoidAsync("set", Key, data);
        }

        public async Task Clear()
        {
            await WaitForReference();
            await _accessorJsRef.Value.InvokeVoidAsync("clear");
        }

        public async Task RemoveAsync(string Key)
        {
            await WaitForReference();
            await _accessorJsRef.Value.InvokeVoidAsync("remove", Key);
        }
    }
}

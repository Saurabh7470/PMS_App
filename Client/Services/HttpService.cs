using Microsoft.JSInterop;
using Newtonsoft.Json;
using System.Data;
using System.Net.Http.Json;
using System.Reflection;
using System.Text.Json;
using System.Text;



namespace Aon_PMS.Services
{
	public class HttpService
	{
        private HttpClient _httpClient;
        private IJSRuntime _JS;
        private Notify _notify;

        public HttpService(HttpClient httpClient, IJSRuntime JS, Notify notify)
		{
            _httpClient = httpClient;
            _JS = JS;
            _notify = notify;
        }
        public void AssignHeader(Guid Company)
        {
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("companyID", Company.ToString());
        }

        public async Task<T> Get<T>(string uri)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, uri);
            return await sendRequest<T>(request);
        }

        public async Task<DataTable> GetDataTable(string uri)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, uri);
            var response = await _httpClient.SendAsync(request);

            await handleErrors(response);

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            string dataString = await response.Content.ReadAsStringAsync();
            return (DataTable)Newtonsoft.Json.JsonConvert.DeserializeObject<DataTable>(dataString);
        }

        public async Task<DataSet> GetDataSet(string uri)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, uri);
            var response = await _httpClient.SendAsync(request);
            //await _notify.Message(JsonSerializer.Serialize(response));
            await handleErrors(response);

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            string dataString = await response.Content.ReadAsStringAsync();
            return (DataSet)Newtonsoft.Json.JsonConvert.DeserializeObject<DataSet>(dataString);
        }

        public async Task PostGetFile(string uri, object value, string Filename)
        {
            var request = createRequest(HttpMethod.Post, uri, value);
            var response = await _httpClient.SendAsync(request);

            await handleErrors(response);

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            Stream dataString = await response.Content.ReadAsStreamAsync();
            using var streamRef = new DotNetStreamReference(stream: dataString);
            await _JS.InvokeVoidAsync("downloadFileFromStream", Filename, streamRef);
        }

        public async Task<DataTable> PostGetDataTable(string uri, object value)
        {
            var request = createRequest(HttpMethod.Post, uri, value);
            var response = await _httpClient.SendAsync(request);

            await handleErrors(response);

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            string dataString = await response.Content.ReadAsStringAsync();
            return (DataTable)Newtonsoft.Json.JsonConvert.DeserializeObject(dataString, (typeof(DataTable)));
        }

        public async Task Post(string uri, object value)
        {
            var request = createRequest(HttpMethod.Post, uri, value);
            await sendRequest(request);
        }

        public async Task<T> Post<T>(string uri, object value)
        {
            var request = createRequest(HttpMethod.Post, uri, value);
            return await sendRequest<T>(request);
        }

        public async Task Put(string uri, object value)
        {
            var request = createRequest(HttpMethod.Put, uri, value);
            await sendRequest(request);
        }

        public async Task<T> Put<T>(string uri, object value)
        {
            var request = createRequest(HttpMethod.Put, uri, value);
            return await sendRequest<T>(request);
        }

        public async Task Delete(string uri)
        {
            var request = createRequest(HttpMethod.Delete, uri);
            await sendRequest(request);
        }

        public async Task<T> Delete<T>(string uri)
        {
            var request = createRequest(HttpMethod.Delete, uri);
            return await sendRequest<T>(request);
        }

        // helper methods

        private HttpRequestMessage createRequest(HttpMethod method, string uri, object value = null)
        {
            var request = new HttpRequestMessage(method, uri);
            string content = Newtonsoft.Json.JsonConvert.SerializeObject(value);

            //_notify.Message(content);
            if (value != null)
                request.Content = new StringContent(content, Encoding.UTF8, "application/json");
            return request;
        }

        private async Task sendRequest(HttpRequestMessage request)
        {
            // send request
            using var response = await _httpClient.SendAsync(request);
            // auto logout on 401 response
            await handleErrors(response);
        }

        private async Task<T> sendRequest<T>(HttpRequestMessage request)
        {
            using var response = await _httpClient.SendAsync(request);

            await handleErrors(response);
            if (typeof(T).ToString() == "System.String")
            {
                var data = await response.Content.ReadAsStringAsync();
                return (T)Convert.ChangeType(data, typeof(T));
            }

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,

                //PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            };
            var d = await response.Content.ReadFromJsonAsync<T>(options);
            return d;
        }

        private async Task<string> sendPostRequest(HttpRequestMessage request)
        {
            using var response = await _httpClient.SendAsync(request);

            await handleErrors(response);

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,

                //PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            };

            return await response.Content.ReadAsStringAsync();
        }

        private static List<T> ConvertDataTable<T>(DataTable dt)
        {
            List<T> data = new();
            foreach (DataRow row in dt.Rows)
            {
                T item = GetItem<T>(row);
                data.Add(item);
            }
            return data;
        }

        private static T GetItem<T>(DataRow dr)
        {
            Type temp = typeof(T);
            T obj = Activator.CreateInstance<T>();

            foreach (DataColumn column in dr.Table.Columns)
            {
                foreach (PropertyInfo pro in temp.GetProperties())
                {
                    if (pro.Name == column.ColumnName)
                        pro.SetValue(obj, dr[column.ColumnName], null);
                    else
                        continue;
                }
            }
            return obj;
        }

        private async Task handleErrors(HttpResponseMessage response)
        {
            // throw exception on error response
            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                error = error ?? response.RequestMessage.ToString();
                await _notify.Message(error);
                //await _dialog.ShowMessageBox("Error", error, yesText: "OK");
            }
        }
    }
}

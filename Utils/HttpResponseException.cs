namespace capybara_api.Utils;

public class HttpResponseException : Exception {
    public int statusCode { get; }
    public string value { get; }

    public HttpResponseException(int statusCode, string value = null) {
        this.statusCode = statusCode;
        this.value = value;
    }
}


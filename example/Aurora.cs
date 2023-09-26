using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

public class Aurora
{
    private readonly string serverAddress;
    private readonly string name;
    private readonly string secret;
    private readonly string hash;
    private readonly string version;

    // Response data structure
    public class Response
    {
        public bool valid { get; set; }
        public string response { get; set; }
    }

    public Response info { get; private set; } // Store API response here

    public Aurora(string name, string secret, string hash, string version, string serverAddress)
    {
        this.name = name;
        this.secret = secret;
        this.hash = hash;
        this.version = version;
        this.serverAddress = serverAddress;
    }

    // Helper function to handle HTTP response
    private string SendGetRequest(string endpoint, Dictionary<string, string> parameters)
    {
        using (var client = new HttpClient())
        {
            var url = serverAddress + endpoint + "?";
            foreach (var param in parameters)
            {
                url += $"{param.Key}={param.Value}&";
            }

            if (parameters.Count > 0)
            {
                url = url.Remove(url.Length - 1);
            }

            var response = client.GetAsync(url).Result;
            response.EnsureSuccessStatusCode();
            return response.Content.ReadAsStringAsync().Result;
        }
    }

    static string UrlEncode(string value)
    {
        return HttpUtility.UrlEncode(value);
    }

    public void connectApi()
    {
        var parameters = new Dictionary<string, string>
        {
            { "action", "init" },
            { "name", name },
            { "secret", secret },
            { "hash", hash },
            { "version", version }
        };

        var response = SendGetRequest("/index.php", parameters);

        // Deserialize JSON response
        var jsonResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(response);

        if (jsonResponse.ContainsKey("error"))
        {
            info = new Response { valid = false, response = jsonResponse["error"] };
        }
        else
        {
            info = new Response { valid = true, response = jsonResponse["message"] };
        }
    }

    public void CheckLicense(string license)
    {
        var parameters = new Dictionary<string, string>
        {
            { "action", "check" },
            { "name", name },
            { "secret", secret },
            { "hash", hash },
            { "version", version },
            { "license", license },
            { "hwid", GetHwid() }
        };

        var response = SendGetRequest("/index.php", parameters);

        // Deserialize JSON response
        var jsonResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(response);

        if (jsonResponse.ContainsKey("error"))
        {
            info = new Response { valid = false, response = jsonResponse["error"] };
        }
        else
        {
            info = new Response { valid = true, response = jsonResponse["message"] };
        }
    }

    public void CheckSession(string sessionId)
    {
        var parameters = new Dictionary<string, string>
        {
            { "action", "check_session" },
            { "id", sessionId }
        };

        var response = SendGetRequest("/index.php", parameters);

        // Deserialize JSON response
        var jsonResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(response);

        if (jsonResponse.ContainsKey("error"))
        {
            info = new Response { valid = false, response = jsonResponse["error"] };
        }
        else
        {
            info = new Response { valid = true, response = jsonResponse["message"] };
        }
    }

    public void KillSession(string sessionId)
    {
        var parameters = new Dictionary<string, string>
        {
            { "action", "kill_session" },
            { "id", sessionId }
        };

        var response = SendGetRequest("/index.php", parameters);

        // Deserialize JSON response
        var jsonResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(response);

        if (jsonResponse.ContainsKey("error"))
        {
            info = new Response { valid = false, response = jsonResponse["error"] };
        }
        else
        {
            info = new Response { valid = true, response = jsonResponse["message"] };
        }
    }

    public void CheckLicenseExpiry(string license)
    {
        var parameters = new Dictionary<string, string>
        {
            { "action", "check_expiry" },
            { "name", name },
            { "secret", secret },
            { "hash", hash },
            { "version", version },
            { "license", license }
        };

        var response = SendGetRequest("/index.php", parameters);

        // Deserialize JSON response
        var jsonResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(response);

        if (jsonResponse.ContainsKey("error"))
        {
            info = new Response { valid = false, response = jsonResponse["error"] };
        }
        else
        {
            info = new Response { valid = true, response = jsonResponse["expiry_date"] };
        }
    }

    public void CheckLicenseSub(string license)
    {
        var parameters = new Dictionary<string, string>
        {
            { "action", "get_license_sub" },
            { "name", name },
            { "secret", secret },
            { "hash", hash },
            { "version", version },
            { "license", license }
        };

        var response = SendGetRequest("/index.php", parameters);

        // Deserialize JSON response
        var jsonResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(response);

        if (jsonResponse.ContainsKey("error"))
        {
            info = new Response { valid = false, response = jsonResponse["error"] };
        }
        else
        {
            info = new Response { valid = true, response = jsonResponse["sub_level"] };
        }
    }

    public string GetVarMessage(string varSecret)
    {
        var parameters = new Dictionary<string, string>
        {
            { "action", "getvar" },
            { "name", name },
            { "secret", secret },
            { "hash", hash },
            { "version", version },
            { "var", varSecret }
        };

        var response = SendGetRequest("/index.php", parameters);

        // Deserialize JSON response
        var jsonResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(response);

        if (jsonResponse.ContainsKey("error"))
        {
            info = new Response { valid = false, response = jsonResponse["error"] };
            return null;
        }
        else
        {
            info = new Response { valid = true, response = jsonResponse["var_message"] };
            return jsonResponse["var_message"];
        }
    }

    public byte[] DownloadFile(string fileSecret)
    {
        var parameters = new Dictionary<string, string>
        {
            { "action", "download" },
            { "name", name },
            { "secret", secret },
            { "hash", hash },
            { "version", version },
            { "file_secret", fileSecret }
        };

        var response = SendGetRequest("/index.php", parameters);

        // Deserialize JSON response
        var jsonResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(response);

        if (jsonResponse.ContainsKey("error"))
        {
            info = new Response { valid = false, response = jsonResponse["error"] };
            return null;
        }

        // Check if the response contains a valid download link
        if (!jsonResponse.ContainsKey("file_link"))
        {
            info = new Response { valid = false, response = "Invalid or missing download link in the response." };
            return null;
        }

        // Get the download link from the response
        var downloadLink = jsonResponse["file_link"];

        using (var client = new HttpClient())
        {
            // Download the file as a byte array
            var downloadResponse = client.GetAsync(downloadLink).Result;
            downloadResponse.EnsureSuccessStatusCode();
            return downloadResponse.Content.ReadAsByteArrayAsync().Result;
        }
    }

    public void SendWebhook(string botName, string iconUrl, string embedTitle, string message)
    {
        var parameters = new Dictionary<string, string>
        {
            { "action", "webhook" },
            { "name", name },
            { "secret", secret },
            { "hash", hash },
            { "version", version },
            { "bot_name", botName },
            { "image", iconUrl },
            { "title", UrlEncode(embedTitle) },
            { "message", UrlEncode(message) }
        };

        var response = SendGetRequest("/index.php", parameters);

        // Deserialize JSON response
        var jsonResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(response);

        if (jsonResponse.ContainsKey("error"))
        {
            info = new Response { valid = false, response = jsonResponse["error"] };
        }
        else
        {
            info = new Response { valid = true, response = jsonResponse["message"] };
        }
    }

    public void banLicnese(string licnese)
    {
        var parameters = new Dictionary<string, string>
        {
            { "action", "ban" },
            { "name", name },
            { "secret", secret },
            { "hash", hash },
            { "version", version },
            { "license", licnese }
        };

        var response = SendGetRequest("/index.php", parameters);

        // Deserialize JSON response
        var jsonResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(response);

        if (jsonResponse.ContainsKey("error"))
        {
            info = new Response { valid = false, response = jsonResponse["error"] };
        }
        else
        {
            info = new Response { valid = true, response = jsonResponse["message"] };
        }
    }

    public void getHwid(string licnese)
    {
        var parameters = new Dictionary<string, string>
        {
            { "action", "get_hwid" },
            { "name", name },
            { "secret", secret },
            { "hash", hash },
            { "version", version },
            { "license", licnese }
        };

        var response = SendGetRequest("/index.php", parameters);

        // Deserialize JSON response
        var jsonResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(response);

        if (jsonResponse.ContainsKey("error"))
        {
            info = new Response { valid = false, response = jsonResponse["error"] };
        }
        else
        {
            info = new Response { valid = true, response = jsonResponse["hwid"] };
        }
    }

    public void licenseNote(string licnese)
    {
        var parameters = new Dictionary<string, string>
        {
            { "action", "get_license_note" },
            { "name", name },
            { "secret", secret },
            { "hash", hash },
            { "version", version },
            { "license", licnese }
        };

        var response = SendGetRequest("/index.php", parameters);

        // Deserialize JSON response
        var jsonResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(response);

        if (jsonResponse.ContainsKey("error"))
        {
            info = new Response { valid = false, response = jsonResponse["error"] };
        }
        else
        {
            info = new Response { valid = true, response = jsonResponse["note"] };
        }
    }

    public void usedDate(string licnese)
    {
        var parameters = new Dictionary<string, string>
        {
            { "action", "get_used_date" },
            { "name", name },
            { "secret", secret },
            { "hash", hash },
            { "version", version },
            { "license", licnese }
        };

        var response = SendGetRequest("/index.php", parameters);

        // Deserialize JSON response
        var jsonResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(response);

        if (jsonResponse.ContainsKey("error"))
        {
            info = new Response { valid = false, response = jsonResponse["error"] };
        }
        else
        {
            info = new Response { valid = true, response = jsonResponse["used_date"] };
        }
    }

     public void getIP(string licnese)
     {
        var parameters = new Dictionary<string, string>
        {
            { "action", "get_license_ip" },
            { "name", name },
            { "secret", secret },
            { "hash", hash },
            { "version", version },
            { "license", licnese }
        };

        var response = SendGetRequest("/index.php", parameters);

        // Deserialize JSON response
        var jsonResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(response);

        if (jsonResponse.ContainsKey("error"))
        {
            info = new Response { valid = false, response = jsonResponse["error"] };
        }
        else
        {
            info = new Response { valid = true, response = jsonResponse["used_date"] };
        }
    }

    private string GetHwid()
    {
        return System.Security.Principal.WindowsIdentity.GetCurrent().User.Value;
    }
}

# ProxyCon

**ProxyCon** is a simple yet flexible proxy connection manager built with .NET 9.0. It allows you to connect and disconnect from various proxy types (HTTP, HTTPS, SOCKS4, SOCKS5) with ease. Whether you're testing network routes or working with privacy tools, ProxyCon helps you route traffic through different proxies and quickly revert to your default network configuration.

---

## Features

- **Connect to multiple proxy types**: Supports HTTP, HTTPS, SOCKS4, and SOCKS5 proxies.
- **Dynamic proxy switching**: You can switch between proxies and easily revert to your default network.
- **Credential support**: Option to enter proxy authentication credentials when required.
- **Simple user interface**: Interact with the application through an easy-to-use terminal interface.
- **Clean connection management**: Easily disconnect from the proxy and revert to default network settings.

---

## Requirements

- **.NET 9.0 or higher**: ProxyCon is built with .NET 9.0, so you need to have the latest .NET SDK installed.
- **Windows/Mac/Linux**: Works across all major platforms where .NET is supported.

---

## Installation

### Prerequisites

1. Install the latest version of the .NET SDK:

   - **Windows/Mac/Linux**: [Download .NET SDK](https://dotnet.microsoft.com/download)

2. Clone this repository to your local machine:

   ```bash
   git clone https://github.com/Rigqx/ProxyCon.git
   ```

3. Navigate to the project folder:

   ```bash
   cd proxycon
   ```

4. Restore dependencies and build the project:

   ```bash
   dotnet build
   ```

---

## Usage

### Running the Program

1. After building, run the program with the following command:

   ```bash
   dotnet run
   ```

2. The program will show a menu with the following options:

   - **Option 1**: Connect to a proxy (choose from HTTP, HTTPS, SOCKS4, SOCKS5).
   - **Option 2**: Disconnect from the proxy and revert to the default network settings.
   - **Option 3**: Exit the program.

3. When connecting to a proxy, you will be prompted for the following details:

   - **Proxy Type**: Choose from `http`, `https`, `socks4`, `socks5`.
   - **Proxy Address**: Enter the proxy's IP address and port (e.g., `127.0.0.1:8080`).
   - **Proxy Credentials**: Optionally enter your username and password for authenticated proxies.
   - **Target URL**: Enter the URL you want to access through the proxy.

4. The program will make a request to the specified URL through the selected proxy and display the status and response content (up to 500 characters).

5. You can switch proxies or disconnect from the proxy at any time.

---

## Example

### Connecting to a SOCKS5 Proxy

```bash
Enter proxy type (http, https, socks4, socks5): socks5
Enter proxy address (e.g., 127.0.0.1:1080): 127.0.0.1:1080
Enter proxy username (leave blank if not needed): user
Enter proxy password (leave blank if not needed): password
Enter the URL to fetch: https://example.com
Sending request through the proxy...
Response Status: OK
Response Content:
<First 500 characters of the response content>
```

### Disconnecting from Proxy

```bash
Reverted to default network settings. Proxy disconnected.
```

---

## How It Works

- **Proxy Handling**: The program uses different proxy classes (HTTP, SOCKS) to route traffic through the specified proxy server.
- **HTTP Client Management**: `HttpClient` is used for making requests. Proxy settings are dynamically applied based on the user input.
- **Clean Network Reset**: When reverting from the proxy, the program creates a new `HttpClient` without any proxy settings, ensuring a clean slate for future network requests.

---

## Troubleshooting

- **Invalid Proxy Address Format**:
  - Ensure that the proxy address is in the correct format: `host:port` (e.g., `127.0.0.1:1080`).
  
- **Proxy Authentication Issues**:
  - Double-check that your proxy username and password are correct.
  
- **Connection Failures**:
  - Ensure that your proxy server is up and reachable, and verify that there are no firewall or network restrictions preventing the connection.

---

## Contributing

Feel free to fork the repository, open issues, or submit pull requests if you’d like to contribute!

1. Fork the repository on GitHub.
2. Create a new branch for your changes.
3. Make the necessary changes and test.
4. Create a pull request with a detailed description of your changes.

---

## License

ProxyCon is licensed under the [MIT License](LICENSE).

---

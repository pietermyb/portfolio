# portfolio
My Portfolio API.

### Portfolio API ###
This project makes use of the following technologies to server an API

- ASPNET.Core
- MySQL
- EF6
- Kestrel
- Swagger UI

#### Enable Https ####
From Windows:
- Open Powershell
  - New-SelfSignedCertificate -certstorelocation cert:\localmachine\my -dnsname yourdomain.org
  - This will output a hash you should later copy
  - $pwd = ConvertTo-SecureString -String "SomeClearTextPassword" -Force -AsPlainText
  - Export-PfxCertificate -cert cert:\localMachine\my\YourCertHashHere -FilePath c:\tmp\portfolio.pfx -Password $pwd

From Ubuntu:
- From Bash
  - openssl req -x509 -newkey rsa:2048 -out cacert.pem -outform PEM -days 1825
  - This will ask you to enter your secure password
  - openssl pkcs12 -export -out portfolio.pfx -in cacert.pem -name "yourdomainname"

```csharp

public static void Main(string[] args)
        {
            var host = new WebHostBuilder()
                .UseKestrel(options =>
                {
                    options.NoDelay = true;
                    //Assign the cert to allow https
                    options.UseHttps("portfolio.pfx", "SomeClearTextPassword");
                    options.UseConnectionLogging();
                    options.AddServerHeader = false; //Sssh. Don't talk to loud.
                })
                 //Serve http and https
                .UseUrls("https://0.0.0.0:5000", "http://0.0.0.0:5001")
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseIISIntegration()
                .UseStartup<Startup>()
                .UseApplicationInsights()
                .Build();

            host.Run();
        }

```

#### Hosting it on Ubuntu ####
- Do a dotnet publish on your project.
- Copy the publish output files to /var/www/portfolioapi
- Create a new portfolio.service file under /etc/systemd/
- Add the following service startup script to portfolio.service
```
[Unit]
    Description=Portfolio.API

    [Service]
    WorkingDirectory=/var/www/portfolioapi/
    ExecStart=/usr/bin/dotnet /var/www/portfolioapi/Portfolio.API.dll
    Restart=always
    RestartSec=10                                          # Restart service after 10 seconds if dotnet service crashes
    SyslogIdentifier=dotnet-portfolio
    User=www-data
    Environment=ASPNETCORE_ENVIRONMENT=Production 

    [Install]
    WantedBy=multi-user.target

```
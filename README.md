## RPN TCP Calculator

#### About

Tcp server and client application that allow the users to:

* Create an account
* Authenticate
* Perform calculations
* View their calculation history
* Report bugs

Written with `.NET Framework 4.7.2`, using a `SQLite` database.

#### Use case diagram

<p align="center">
  <img src="/media/DiagramPrzypadkow.png">
</p>

#### Activity diagrams

* Assigning admin role in server GUI application

<p align="center">
  <img src="/media/AdminRoleGiving.png" width=350>
</p>

* Assigning admin role in client application

<p align="center">
  <img src="/media/ActivityAdminRoleInClient.png" width=350>
</p>

* Change password

<p align="center">
  <img src="/media/ActivityChpwd.png" width=350>
</p>

* Send calculation request

<p align="center">
  <img src="/media/ActivityCalculate.png" width=350>
</p>

* Review bug reports

<p align="center">
  <img src="/media/ActivityReviewReports.png" width=500>
</p>

#### Sequence diagrams

* Change password

<p align="center">
  <img src="/media/SequenceChpwd.png">
</p>

* Assigning admin role in client application

<p align="center">
  <img src="/media/SequenceAdmin.png">
</p>

#### Deployment diagram

<p align="center">
  <img src="/media/DiagramWdrozenia.png">
</p>

#### Class diagram

<p align="center">
  <img src="/media/DiagramKlas.png">
</p>

#### Client screenshots

<p align="center">
  <img src="/media/screen1.PNG">
</p>

<p align="center">
  <img src="/media/screen2.PNG">
</p>

<p align="center">
  <img src="/media/screen3.PNG">
</p>

<p align="center">
  <img src="/media/screen4.png">
</p>

<p align="center">
  <img src="/media/screen5.png">
</p>

<p align="center">
  <img src="/media/screen6.png">
</p>

<p align="center">
  <img src="/media/screen7.png">
</p>

#### Selected code fragments

* Client - function handling user authentication

```csharp
public static async Task HandleAuthenticationProcedure(NetworkStream stream,
    AuthProcedure authProcedure,
    string username,
    string password,
    string newPassword = null)
{
    var streamReader = new StreamReader(stream);

    if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
        throw new ArgumentNullException("fields cannot be blank");

    _ = await streamReader.ReadLineAsync(); //You are connected
    _ = await streamReader.ReadLineAsync(); //Please authenticate

    string procedure = string.Empty, request = null;

    switch (authProcedure)
    {
        case AuthProcedure.Login:
            procedure = CoreLocale.Login;
            break;
        case AuthProcedure.Register:
            procedure = CoreLocale.Register;
            break;
        case AuthProcedure.ChangePassword:
            procedure = CoreLocale.ChangePassword;
            request = $"{procedure} {username} {password} {newPassword}";
            break;
    }

    await SendToStreamAsync(stream, request ?? $"{procedure} {username} {password}");

    var message = await streamReader.ReadLineAsync(); //Enter RPN Expression / Error

    switch (message)
    {
        case CoreLocale.UserLoggedIn:
            throw new DuplicateNameException(message);
        case CoreLocale.UsernameTaken:
            throw new DuplicateNameException(message);
        case CoreLocale.NoSuchUsername:
            throw new InvalidCredentialException(message);
        case CoreLocale.InvalidPassword:
            throw new InvalidCredentialException(message);
    }

    _ = await streamReader.ReadLineAsync(); //'history' to check last inputs
    _ = await streamReader.ReadLineAsync(); //'exit' to disconnect
    _ = await streamReader.ReadLineAsync(); //'report <message>' to report a problem
}
```

* Server GUI Client

```csharp
private async void startButton_Click(object sender, EventArgs e)
{
      try
      {
           SetVisibility(_buttons, true);
           startButton.Visible = false;
           IPAddress ipAddress = null;
           var port = 0;

           if (ipBox.TextLength.Equals(0) && portBox.TextLength.Equals(0)){     
                ipAddress = IPAddress.Loopback;
                port = 1024;
           }else{
                ipAddress = IPAddress.Parse(ipBox.Text);
                port = int.Parse(portBox.Text);
           }

           if (ipAddress == null || port == 0){
                Console.WriteLine("Please enter following arguments: 1. IP address, 2. port");
                return;
           }

           Console.WriteLine(ipAddress.ToString());

           var contextBuilder = new ContextBuilder(_ctrlWriter.Write);

           _rpnServer = new ResponseServerAsync(ipAddress, port, RPNCalculator.GetResult, Encoding.ASCII,
           contextBuilder.CreateRpnContext, _ctrlWriter.Write);

           if (!isRunned){
               _rpnServer.Start();
               isRunned = true;
           }else
               _ctrlWriter.Write("[Alert] Server is started!");
      }
      catch (Exception)
      {
              _ctrlWriter.Write("[Alert] Start server to use implemented options");
      }
}
```

* Server

```csharp
private Task Send(NetworkStream stream, IEnumerable<object> models)
{
        return Send(stream, models.Select(m => m.ToString()));
}

private Task Send(NetworkStream stream, IEnumerable<string> lines)
{
        return Send(stream, string.Join("\r\n", lines));
}

private Task Send(NetworkStream stream, string message)
{
        var messageLine = $"{message}\r\n";
        return stream.WriteAsync(_encoding.GetBytes(messageLine), 0, messageLine.Length);
}
```
> 2020 @ PUT

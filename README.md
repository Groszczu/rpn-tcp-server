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

```

* Server

```csharp

```
> 2020 @ PUT

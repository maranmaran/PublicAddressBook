# Public address book
<p>The app should be implemented using .NET Core Web API and PostgreSQL database.</p>
<p>Rest of technologies required to implement solution are your choice.</p>

### Features:
<ul>
  <li>Contacts need to contain name, date of birth, address and multiple telephone numbers</li>
  <li>Contacts need to be unique by name and address</li>
  <li>Creating, updating and deleting contacts</li>
  <li>Providing access to single and multiple contacts (with pagination)</li>
  <li>Provide a way of receiveing live updates for connected clients (signalR, websockets)</li>
</ul>

### Start
<p>Clone repo</p>
<p>Make sure you have admin rights</p>

# Backend project:
## Technologies: 
* .Net Core 3.1
* SignalR
* PostgreSQL
* EF Core
* MediatR
* Fluent validation
* Automapper
* Audit.NET

## Architecture
![](Architecture2.png)
<br/>

## Build and run:
* Open project
* Set Backend.API project as startup
* Create appSettings.Development.json -> Use appSettings.json as cook-book
* Start http://localhost:65389/api/index.html
* Database should automatically seed.

### To run test
* Open test project and run unit tests or write `dotnet test`

#### Coverage:
![](Covereage.PNG)
<br/>

# Frontend project:

## Technologies:
* Angular 9
* Angular material
* NGRX
* RxJS

## Build and run:
* Open project
* Run "npm install" to setup dependencies
* Create environment.dev.ts -> Use environment.ts as cook-book. 
  * You will need to provide API key for phone validtion service. 
  * I can provide that if you PM me or open your account on   https://numverify.com/</li>
* Run "ng serve -c dev"</li>
* Start</li>
* Public contacts: http://localhost:4200/contacts
* Login: http://localhost:4200/auth/login (Username: user, Password: user)


# Demo
>>Video here<<

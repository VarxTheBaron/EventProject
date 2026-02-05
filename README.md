# EventHomepage MVC

Förslag på steg för steg vad du kan behöva göra:

1. Skapa mappen EventHomepage och i den ett nytt MVC-projekt
1. Lägg till ditt MVC-projekt till din solution med Add existing project
2. Lägg till **EFCore Sqlite** till EventHomepage så att du kan konfigurera en databas i Program.cs
3. Se till så att projektet refererar EventCore och EventInfrastructure så att du kan använda dina domänmodeller och databasen
4. Fixa migrations:
 * Installera EF Core tools: `dotnet tool install --global dotnet-ef`
 * Se till att stå i src-mappen och kör dessa två kommandon:
    * `dotnet ef migrations add InitialCreate --project "EventInfrastructure\EventInfrastructure.csproj" --startup-project "EventHomepage\EventHomepage.csproj"`
    * `dotnet ef database update --project "EventInfrastructure\EventInfrastructure.csproj" --startup-project "EventHomepage\EventHomepage.csproj"`
    Det första kommandot skapar dina migrations-filer (Titta i dem så ser du vad de gör!)
    Det andra kommandot applicerar migrationen på databasen, vilket i det här fallet skapar en SQLite-databasfil med rätt tabeller.
5. Börja med att skapa API-endpointen för att lägga till events, så att du kan testa hela vägen från controller, via service, till databas. Detta kan du göra med en enkel MapPost i program.cs eller så skapar du en controller, eller så gör du extensionmethods som vi testade tidigare.
    * Skapa ett DTO-objekt för att ta emot data för ett nytt event. Titta i Event.cs i EventCore för att se vilka properties du kan tänkas behöva. 
    * Vänta med PUT och DELETE.
6. Uppdatera HomeControllern och dess Index-vy så att du kan lista alla events på startsidan. Visa namn, tid, plats, om det är fullt eller inte, kanske hur många platser som är kvar. GÖR DET INTE SNYGGT! Fokusera på att få in datan och visa den, så kan du fixa designen senare.
    * Se till att skapa en ViewModel (Kan heta tex EventListViewModel) som kan hålla den information som du vill visa om ett event på första sidan. Använd din dbContext med Select för att mappa från Event-entities till en lista med dina view models du just skapade.
7. Fixa en knapp/länk för att gå till den detaljerade vyn för eventen.
    * Lägg till en vy för detaljerad info.
    * Denna detaljerade vy kommer möjligen att vilja ha sin egna viewmodel (EventDetailsViewModel) eller så gör du en som används i båda fallen bara att du väljer att visa olika mycket från den.
8. Den detaljerade vyn ska ha en formulär där man kan registrera sig för ett event. För att se hur detta sker i domänen måste du undersöka Event.cs. Du måste nu sätta upp en Action som kan ta emot formulärens data. 
    * Skapa ett DTO-objekt för event-registrering (Titta i Registration.cs för hints).

Sammanfattning av viktiga filer som du kan behöva skapa:

### Controllers som du behöver
* HomeController.cs - Din mvc-controller
* EventsController.cs - Din api-controller, om du väljer det upplägget

### View
* Index.cshtml - För att lista alla events
* Details.cshtml - För att visa detaljerad info om ett event och ha registreringsformuläret

### DTOs för API (Kan vara records)
* CreateEventDto.cs - för att skapa events via API
* EventDto.cs - eventdata med antal registreringar och full-status

### ViewModels för MVC (Klasser med validering)
* EventViewModel.cs - för att listan med events på startsidan
* EventDetailsViewModel.cs - för detaljerad eventvy
* RegisterViewModel.cs - ta emot från formuläret

## Tips

* Kolla upp `RedirectToAction()` för att förstå hur du kan skicka användaren till en annan action efter tex en lyckad registrering (alltså tillbaka till eventets detaljsida).
* Undersök hur `TempData` fungerar i MVC så att du enkelt kan skicka med ett meddelande som säger "Du är nu registrerad!" när du skickar tillbaka användaren till detaljsidan.
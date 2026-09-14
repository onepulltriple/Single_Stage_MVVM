# SingleStage

SingleStage is a desktop WPF application using MVVM architecture written in C# targeting .NET 8. It manages a single-stage entertainment venue's schedule and administrative data (Shows, Performances, Artists, Seats, Tickets, Ticketholders). The project follows a database-first approach. Entity Framework Core scaffolded entity classes and a small DAC (Data Access Component) layer encapsulate data operations. 

A calendar week overview displaying the venue's schedule is the main application screen.

## How to Run 

This application uses SQL Server Express and expects a SQL Server instance named `SQLEXPRESS`.

1. Install [SQL Server Express](https://www.microsoft.com/en-us/download/details.aspx?id=104781&lc=1033).

   During installation, create/use an instance named `SQLEXPRESS`.

2. Install [SQL Server Management Studio (SSMS)](https://learn.microsoft.com/en-us/ssms/install/install).

3. Open SSMS and connect to the SQL Server instance using:

   - **Server type:** Database Engine
   - **Server name:** `.\SQLEXPRESS`
   - **Authentication:** Windows Authentication

    The application uses the same SQL Server instance and Windows authentication to connect to the database. No SQL Server username or password needs to be entered in the application.

4. In SSMS, open and run the
   [database setup script](https://github.com/onepulltriple/Single_Stage_MVVM/blob/main/Database/Scaffolds/002.01%20-%20Initial%20Database%20Setup%20-%20Demo.sql).

   This creates the `Single_Stage_MVVM` database.

5. Verify in SSMS that the `Single_Stage_MVVM` database appears under **Databases**.

6. If you want to run the application with sample data, open and run the
   [database seed script](https://github.com/onepulltriple/Single_Stage_MVVM/blob/main/Database/Scaffolds/002.02%20-%20Populate%20Database%20-%20Demo.sql).

   If you want to test the application with an empty database, skip this step.

7. Install Visual Studio. [Download 2026](https://visualstudio.microsoft.com/downloads/)

   During installation, make sure the **.NET desktop development** workload is selected. This is required for the WPF application.

8. Clone the repository to your local machine using:

   ```bash
   git clone https://github.com/onepulltriple/Single_Stage_MVVM
   ```

9. Open the `Single_Stage_MVVM.sln` in Visual Studio.

10. Build and run the application in Visual Studio.

    The application connects to SQL Server using the following connection string:
    `Server=.\SQLEXPRESS;Database=Single_Stage_MVVM;Integrated Security=True;TrustServerCertificate=True`

    This means the application expects SQL Server Express to be running locally as the `SQLEXPRESS` instance and uses your Windows credentials to connect. No SQL Server username or password needs to be entered in the application.

    If your SQL Server instance has a different name: update the `Server=` portion of the connection string in `Entities/SingleStageMvvmContext.cs` to match the server name you use to connect in SSMS. For example, if SSMS connects using localhost, use 
    `Server=localhost;Database=Single_Stage_MVVM;Integrated Security=True;TrustServerCertificate=True`

11. Once the app is running, log in with username `guest` and password `stage`.

#### Troubleshooting

**Cannot connect to SQL Server:** 
Make sure the SQL Server (`SQLEXPRESS`) Windows service is running and that the server name in `SingleStageMvvmContext.cs` matches the server name you used in SSMS.

**NETSDK1045: The current .NET SDK does not support targeting .NET 8.0:**
Make sure the .NET 8 SDK is installed. Run dotnet --list-sdks in PowerShell or Command Prompt and verify that an 8.0.xxx SDK is listed. If it is not, install the .NET 8 SDK from Microsoft's .NET 8 [download page](https://dotnet.microsoft.com/en-us/download/dotnet/8.0), then restart Visual Studio. 


## Using the application

The following is an example of an end-to-end walkthrough from planning a full evening show to printing that show's flyer. The workflow in the application is roughly:

**Log in → Create artists → Create a show → Add performances → Assign artists → Print flyer**


#### Step 0 — Log in

Start the application and log in through the Employee Login window. The username is 'guest' and the password is 'stage'. After successful login, the main window opens.

The upper portion of the main window is the weekly calendar. Initially, the user is primarily interested in getting a show and its lineup into the database. The calendar becomes especially useful once the show has been scheduled.


#### Step 1 — Create artists

In the upper left docked menu, go to Manage → Manage Artists. This opens the separate Manage Artists window.

Create a new artist by clicking 'New' and entering the artist's name.

For example, enter: "Melancholy Michael" and click 'Save' or press the Enter key.

Create two more artists: "Czech Josef" and "The Vinyl Brothers".

When finished, close the Manage Artists window and return to the main window.


#### Step 2 — Create a show

In the upper left docked menu, go to Manage → Manage Shows.

Create a new show by clicking 'New' and entering the information below.

- Name:
    Thursday Night Variety

- Start:
    Thursday, October 8, 2026 at 19:00

- End:
    Thursday, October 8, 2026 at 23:30

- Ticket price:
    25.00

Click 'Save' and close the window, or click 'Manage selected show's performances' in the bottom right.

The show has been created, but it doesn't have any performances yet.


#### Step 3 — Add performances to a show

In the Manage Performances window, it is possible to create performances for a show and then assign artists to those performances.

If not already selected, choose the recently created show from the "Filter by show:" selection box. 

Create the first performance by clicking "New" and entering the information below.

- Description:
    Comedy Opening Double Act

- Start:
    Thursday, October 8, 2026 at 19:00

- End:
    Thursday, October 8, 2026 at 20:45

Save this performance by clicking 'Save' or pressing the Enter key. Keep this window open.

Create a second performance:

- Description:
    Music Headliner

- Start:
    Thursday, October 8, 2026 at 21:30

- End:
    Thursday, October 8, 2026 at 23:30

Save this performance by clicking 'Save' or pressing the Enter key. Keep this window open.


#### Step 4 — Assign artists to performances

In the upper right of the Manage Performances window, switch the management mode to the "Assign artists" mode by clicking the large toggle button.

Assign 'Melancholy Michael' to 'Comedy Opening Double Act' by ensuring the 'Comedy Opening Double Act' performance is selected, then click 'New'. Enter the information below.

- Artist:
    Choose 'Melancholy Michael' from the selection box at the upper right.

- Royalty up front:
    100

- Royalty at end:
    200

Save this artist assignment by clicking 'Save' or pressing the Enter key. The artist should be listed beneath the 'Comedy Opening Double Act' performance in the list on the left of the screen.

Repeat these steps to create a second artist assignment for 'Comedy Opening Double Act'. Enter the information below.

- Artist:
    Choose 'Czech Josef' from the selection box at the upper right.

- Royalty up front:
    100

- Royalty at end:
    200

Save this artist assignment by clicking 'Save' or pressing the Enter key. The artist should be listed beneath the 'Comedy Opening Double Act' performance in the list on the left of the screen.

Finally, assign 'The Vinyl Brothers' to 'Music Headliner' by ensuring the 'Music Headliner' performance is selected, then click 'New'. Enter the information below.

- Artist:
    Choose 'The Vinyl Brothers' from the selection box at the upper right.

 - Royalty up front:
    300

 - Royalty at end:
    300

Save this artist assignment by clicking 'Save' or pressing the Enter key. The artist should be listed beneath the 'Music Headliner' performance in the list on the left of the screen.

Close the Manage Performances window.


#### Step 5 - Print the show's flyer

Now that the show's full line-up has been created, it is time to let customers know about it.

Return to the home screen and find the show in the calendar. Use the date picker to jump to the correct date. Alternatively, use the navigation arrows or press the left or right arrows on the keyboard.

- Name:
    Thursday Night Variety

- Start:
    Thursday, October 8, 2026 at 19:00

- End:
    Thursday, October 8, 2026 at 23:30

Select this show to see some brief information at the bottom of the home screen.

With this show still selected, go to the upper left docked menu and select 'Print' → 'Print flyer'. A window will appear with the show's line-up information.

If changes to the show are needed, double-click on the show in the home screen.
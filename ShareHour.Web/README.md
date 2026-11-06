# Share Hour | COMP4701 Phase 1

ASP.NET Core 8 Razor Pages + Entity Framework Core SQLite + Bootstrap 5.3.6 + jQuery 3.6.1.

## Run in Visual Studio 2026
1. Double-click **`ShareHour.sln` in the extracted ZIP root** to open the full solution in Visual Studio. Ensure the **ASP.NET and web development** workload and **.NET 8 SDK** are installed.
2. Restore NuGet packages (requires access to nuget.org on first use) and build.
3. In Solution Explorer, right-click `ShareHour.Web` → **Set as Startup Project** if necessary. Choose the `ShareHour.Web` launch profile, then press **Ctrl+F5**. The app is configured to open `http://localhost:5086`.
4. A SQLite `sharehour.db` file is created automatically when the app first starts.
5. Demo accounts: `owner@sharehour.test` and `renter@sharehour.test`; both passwords `Demo123!`.
6. Owner: list a tool and review bookings. Renter: browse, request a booking, and cancel a pending request.

## Critical caveats
- The application has NOT been compiled or run in the document-generation environment. You must verify it locally before claiming test passes.
- Demo passwords are only for the classroom; remove demo accounts and strengthen security before publication.
- `EnsureCreated` is a teaching shortcut, not a database migration strategy.
- SQLite stores booking requests, tool details and accounts. No email or SMS notification integration is provided; owners see new pending requests in their dashboard.
- Booking overlap validation uses a check-before-save workflow; a production app requires transactional locking/concurrency protection against simultaneous submissions.
- Bootstrap and jQuery are bundled under `wwwroot/lib`, so the UI works without internet once NuGet has been restored.

## Screenshot checklist for the report
01 home catalog (browser URL visible)
02 login and both roles (no sensitive values)
03 owner creates tool
04 renter enters booking times and estimated cost
05 invalid/overlapping booking error
06 renter pending booking
07 owner request and approval
08 approved booking seen by renter
09 Visual Studio Solution Explorer showing code
10 GitHub public repository showing project files and team contributions

## Report notes
Two real user interviews, actual contribution percentages, measured NFR trials, the recorded demo and published repository URL must be supplied by the team. They are not fabricated.

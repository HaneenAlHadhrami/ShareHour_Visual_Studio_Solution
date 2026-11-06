# Manual acceptance tests (not yet executed)

Use the two seeded demo accounts and record actual results and screenshots in the report.

| Test ID | Steps | Expected result |
|---|---|---|
| TC-01 | Open `/` | Three sample tools visible |
| TC-02 | Search `drill` | Only matching tools displayed |
| TC-03 | Sign in as owner, list tool | New tool appears in catalog |
| TC-04 | Sign in as renter, submit 3-hour future booking | Pending booking created with hourly price x 3 |
| TC-05 | Submit past booking | Validation error; no booking |
| TC-06 | Submit end time earlier than start time | Validation error; no booking |
| TC-07 | Request same tool with overlapping period | Overlap error; no second booking |
| TC-08 | Sign in as owner, approve pending | Status changes to Approved |
| TC-09 | Sign in as renter, view bookings | Approved status shown |
| TC-10 | Anonymous user opens `/Bookings/Create?toolId=1` | Redirect to sign-in |
| TC-11 | Renter visits `/Bookings/Review` | Access denied |
| TC-12 | Refresh after creating booking | Data still exists in SQLite |
| TC-13 | Renter cancels pending booking | Status becomes Cancelled |
| TC-14 | Submit invalid email during registration | Validation shown |

Additional NFR trials (from earlier Software Testing draft): 5 loading trials <= 5 sec; 5 booking trials <= 5 sec; 5 access attempts denied; 5 saved bookings retained after refresh; 3 first-time users complete a booking within 5 minutes. Record measured results, not assumptions.

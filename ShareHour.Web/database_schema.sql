-- Conceptual relational schema consistent with the three EF Core entities.
-- EF Core EnsureCreated generates SQLite tables at app startup.
CREATE TABLE Users (
    Id INTEGER PRIMARY KEY,
    FullName TEXT NOT NULL,
    Email TEXT NOT NULL UNIQUE,
    PasswordHash TEXT NOT NULL,
    Role TEXT NOT NULL CHECK(Role IN ('Renter','Owner'))
);
CREATE TABLE Tools (
    Id INTEGER PRIMARY KEY,
    OwnerId INTEGER NOT NULL REFERENCES Users(Id),
    Name TEXT NOT NULL,
    Description TEXT NOT NULL DEFAULT '',
    Category TEXT NOT NULL,
    HourlyPrice NUMERIC NOT NULL CHECK(HourlyPrice > 0),
    IsActive INTEGER NOT NULL DEFAULT 1
);
CREATE TABLE Bookings (
    Id INTEGER PRIMARY KEY,
    ToolId INTEGER NOT NULL REFERENCES Tools(Id),
    RenterId INTEGER NOT NULL REFERENCES Users(Id),
    StartAt TEXT NOT NULL,
    EndAt TEXT NOT NULL,
    TotalCost NUMERIC NOT NULL,
    Status TEXT NOT NULL CHECK(Status IN ('Pending','Approved','Rejected','Cancelled')),
    CreatedAt TEXT NOT NULL,
    CHECK(EndAt > StartAt)
);

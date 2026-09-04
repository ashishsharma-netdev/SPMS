IF OBJECT_ID(N'dbo.Students', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Students
    (
        Id INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Students PRIMARY KEY,
        FirstName NVARCHAR(50) NOT NULL,
        LastName NVARCHAR(50) NOT NULL,
        Email NVARCHAR(150) NOT NULL,
        Mobile NVARCHAR(20) NOT NULL,
        DateOfBirth DATETIME2 NOT NULL,
        Gender NVARCHAR(20) NOT NULL,
        Course NVARCHAR(100) NOT NULL,
        Address NVARCHAR(250) NOT NULL,
        City NVARCHAR(80) NOT NULL,
        State NVARCHAR(80) NOT NULL,
        Pincode NVARCHAR(6) NOT NULL,
        RegistrationDate DATETIME2 NOT NULL,
        IsActive BIT NOT NULL CONSTRAINT DF_Students_IsActive DEFAULT 1
    );
    CREATE UNIQUE INDEX IX_Students_Email ON dbo.Students(Email);
END

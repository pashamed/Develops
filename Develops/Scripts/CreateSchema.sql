CREATE TABLE [dbo].[TaxiTrip] (
    [PickupDateTime] DATETIME2 NOT NULL,
    [DropoffDateTime] DATETIME2 NOT NULL,
    [PassengerCount] TINYINT NOT NULL,
    [TripDistance] DECIMAL(5, 2) NOT NULL,
    [StoreAndFwdFlag] NVARCHAR(3) NOT NULL,
    [PULocationID] INT NOT NULL,
    [DOLocationID] INT NOT NULL,
    [FareAmount] DECIMAL(5, 2) NOT NULL,
    [TipAmount] DECIMAL(5, 2) NOT NULL,
    CONSTRAINT [PK_TaxiTrip] PRIMARY KEY (
        [PickupDateTime],
        [DropoffDateTime],
        [PassengerCount]
    )
);

CREATE INDEX [IX_TaxiTrip_PULocationID_TipAmount]
ON [dbo].[TaxiTrip] ([PULocationID], [TipAmount]);

CREATE INDEX [IX_TaxiTrip_TripDistance]
ON [dbo].[TaxiTrip] ([TripDistance] DESC);

ALTER TABLE [dbo].[TaxiTrip]
ADD [TravelTime] AS DATEDIFF(SECOND, [PickupDateTime], [DropoffDateTime]);

CREATE INDEX [IX_TaxiTrip_TravelTime]
ON [dbo].[TaxiTrip] ([TravelTime] DESC);

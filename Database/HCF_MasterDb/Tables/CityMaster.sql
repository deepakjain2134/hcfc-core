CREATE TABLE [dbo].[CityMaster] (
    [CityId]      INT            IDENTITY (1, 1) NOT NULL,
    [CityName]    NVARCHAR (100) NOT NULL,
    [CityCode]    NVARCHAR (50)  NOT NULL,
    [StateId]     INT            NOT NULL,
    [IsActive]    BIT            NULL,
    [CreatedBy]   INT            NULL,
    [CreatedDate] DATETIME       NULL,
    [Zipcode]     INT            DEFAULT ((0)) NULL
);


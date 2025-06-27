USE [bankManagementDB]
GO

/****** Object:  StoredProcedure [dbo].[sp_ValidateClient]    Script Date: 6/23/2025 4:00:43 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[sp_ValidateClient]
    @Username VARCHAR(100),
    @Password VARCHAR(100)
AS
BEGIN
    SELECT * FROM client_master_table
    WHERE Username = @Username AND Password = @Password
END
GO


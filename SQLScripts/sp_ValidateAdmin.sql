USE [bankManagementDB]
GO

/****** Object:  StoredProcedure [dbo].[sp_ValidateAdmin]    Script Date: 6/25/2025 9:48:42 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[sp_ValidateAdmin]
    @Username VARCHAR(50),
    @Password VARCHAR(50)
AS
BEGIN
    SELECT * FROM admin_master_table
    WHERE username = @Username AND password = @Password
END
GO


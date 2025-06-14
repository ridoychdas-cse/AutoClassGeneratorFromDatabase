IF OBJECT_ID('dbo.sp_GenerateClassProperties', 'P') IS NULL
BEGIN
    EXEC('
    CREATE PROCEDURE [dbo].[sp_GenerateClassProperties]
    AS
    BEGIN
        SET NOCOUNT ON;

        SELECT 
            STRING_AGG(PropertyText, CHAR(13) + CHAR(10)) AS PropertyTexts,
            ClassName
        FROM 
        (
            SELECT 
                ''public '' + 
                CASE 
                    WHEN DATA_TYPE IN (''bigint'') THEN ''long''
                    WHEN DATA_TYPE IN (''int'') THEN ''int''
                    WHEN DATA_TYPE IN (''tinyint'') THEN ''byte''
                    WHEN DATA_TYPE IN (''smallint'') THEN ''short''
                    WHEN DATA_TYPE IN (''bit'') THEN ''bool''
                    WHEN DATA_TYPE IN (''varchar'', ''nvarchar'', ''char'', ''nchar'', ''text'') THEN ''string''
                    WHEN DATA_TYPE IN (''datetime'', ''datetime2'', ''smalldatetime'', ''date'') THEN ''DateTime''
                    WHEN DATA_TYPE IN (''decimal'', ''money'', ''numeric'', ''smallmoney'') THEN ''decimal''
                    WHEN DATA_TYPE IN (''float'') THEN ''double''
                    WHEN DATA_TYPE IN (''real'') THEN ''float''
                    ELSE ''object''
                END + 
                CASE 
                    WHEN IS_NULLABLE = ''YES'' 
                         AND DATA_TYPE NOT IN (''varchar'', ''nvarchar'', ''text'', ''char'', ''nchar'') 
                    THEN ''?'' ELSE '''' 
                END + '' '' +
                COLUMN_NAME + '' { get; set; }'' AS PropertyText,
                DATA_TYPE,
                TABLE_NAME AS ClassName,
                TABLE_SCHEMA
            FROM INFORMATION_SCHEMA.COLUMNS
            WHERE TABLE_NAME IN (
                SELECT TABLE_NAME 
                FROM INFORMATION_SCHEMA.TABLES 
                WHERE TABLE_TYPE = ''BASE TABLE''
            )
        ) T1
        GROUP BY T1.ClassName
    END
    ')
END

# NHibernateLogToSQLParameter
A simple tool for myself to fast convert NHibernate SQL parameter logs to SQL Server types

## Usage

This is mainly a convenience tool for myself while debugging the NHibernate ORM.

This convert NHibernate parameters in logs from this format:

```
@p0 = xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx [Type: Guid (0:0:0)],
@p1 = 'Something' [Type: String (4000:0:0)],
@p2 = 'SomethingElse' [Type: String (4000:0:0)],
@p3 = False [Type: Boolean (0:0:0)],
@p4 = 1 [Type: Int32 (0:0:0)],
```

To this:

```sql
DECLARE @p0 UNIQUEIDENTIFIER = 'xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx';
DECLARE @p1 NVARCHAR(4000) = 'Something';
DECLARE @p2 NVARCHAR(4000) = 'SomethingElse';
DECLARE @p3 BIT = 0;
DECLARE @p4 INT = 1;
```

This project has been made in 30 minutes in order to gain more time in the long run, but so this mean that some things may not be managed yet, like dynamic values for the String type, missing types too. I will update depending my needs !

Should be used like this:

```bash
NHibernateLogToSQLParameter.exe "fullPathToSourceFile" "fullPathToDestinationFile"
```

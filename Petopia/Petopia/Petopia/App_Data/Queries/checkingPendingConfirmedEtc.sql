--ALTER TABLE Persons
--ALTER COLUMN DateOfBirth year;
---------------------------------------------------------------------
--ALTER TABLE CareTransaction
--ADD Completed_CP bit;
---------------------------------------------------------------------
--UPDATE CareTransaction SET Pending = 0 WHERE PetOwnerID=19;

select TransactionID, PetOwnerID, Pending, Confirmed, Completed_PO, Completed_CP from CareTransaction where PetID = 24;
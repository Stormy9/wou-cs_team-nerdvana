--ALTER TABLE Persons ALTER COLUMN DateOfBirth year;
---------------------------------------------------------------------
--ALTER TABLE CareTransaction ADD Completed_CP bit;
---------------------------------------------------------------------
--UPDATE CareTransaction SET Pending = 0 WHERE PetOwnerID=19;
-----------------------------------------------------------------------
select TransactionID, StartDate, ct.PetOwnerID, ct.PetID, ct.CareProviderID, NeededThisVisit, Pending, Confirmed
from CareTransaction as ct
                
                join CareProvider as cp on ct.CareProviderID = cp.CareProviderID
                join PetOwner as po on ct.PetOwnerID = po.PetOwnerID
                join PetopiaUsers as puO on po.UserID = puO.UserID
                join PetopiaUsers as puP on cp.UserID = puP.UserID
                join Pet as p on ct.PetID = p.PetID

				where (Pending = 1 and Confirmed = 0)
					and (ct.PetOwnerID = 9 or ct.CareProviderID = 7)
				order by PetID;
-----------------------------------------------------------------------
--delete from CareTransaction;
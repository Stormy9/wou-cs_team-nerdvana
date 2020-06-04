--select TransactionID as T_ID, CareProviderID as CP_ID, PetID, PetOwnerID as POID, 
--		Pending, Confirmed, Completed_PO as Cmp_PO, Completed_CP as Cmp_PC, PC_Rating, PO_Rating 
--from CareTransaction;
-----------------------------------------------------------------------------------------------------------
--select avg(PC_Rating) as CarerRating, avg(PO_Rating) as OwnerRating from CareTransaction;

--ALTER TABLE CareProvider ALTER COLUMN AverageRating decimal(3,2);
-----------------------------------------------------------------------------------------------------------
select * from AspNetUsers where Email = 'bleh@bleh.com';
select * from PetopiaUsers where ASPNetIdentityID = '1e368e90-01f7-42c8-8727-3262163ad4a1';
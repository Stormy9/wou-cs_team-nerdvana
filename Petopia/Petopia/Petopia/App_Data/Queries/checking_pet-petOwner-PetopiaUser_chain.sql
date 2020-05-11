--SELECT t1.col, t3.col FROM table1 join table2 ON table1.primarykey = table2.foreignkey
--                                  join table3 ON table2.primarykey = table3.foreignkey
-----------------------------------------------------------------------------------------------------------
select Pet.PetID, Pet.PetName, Pet.Breed, Pet.Birthdate, Pet.PetOwnerID as Pets_OwnerID, 
		PetOwner.PetOwnerID, PetOwner.GeneralNeeds, PetOwner.UserID as PO_UID, 
		PetopiaUsers.UserID as PU_UID, PetopiaUsers.FirstName, PetopiaUsers.LastName, 
			PetopiaUsers.GeneralLocation, PetopiaUsers.ASPNetIdentityID
		FROM
		Pet join PetOwner on Pet.PetOwnerID = PetOwner.PetOwnerID
			join PetopiaUsers on PetOwner.UserID = PetopiaUsers.UserID
		WHERE
		Pet.PetID = 12;		-- checked PetID = 11 first
-- double-checked on PetOwnerID = 9 (my testing account w/4 pets)   [=
-----------------------------------------------------------------------------------------------------------
select Pet.PetID, Pet.PetName, Pet.Breed, Pet.Birthdate, Pet.PetOwnerID as Pets_OwnerID, 
		PetOwner.PetOwnerID, PetOwner.GeneralNeeds, PetOwner.UserID as PO_UID, 
		PetopiaUsers.UserID as PU_UID, PetopiaUsers.FirstName, PetopiaUsers.LastName, 
			PetopiaUsers.GeneralLocation, PetopiaUsers.ASPNetIdentityID
		FROM
		Pet join PetOwner on Pet.PetOwnerID = PetOwner.PetOwnerID
			join PetopiaUsers on PetOwner.UserID = PetopiaUsers.UserID
		WHERE
		PetOwner.PetOwnerID = 19;
		--PetopiaUsers.UserID = 19;
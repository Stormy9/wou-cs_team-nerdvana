--select UserID, FirstName, LastName, ResZipCode 
--select PetopiaUsers.UserID, PetopiaUsers.IsProvider, PetopiaUsers.FirstName, PetopiaUsers.LastName, UserBadge.DogProvider, UserBadge.CatProvider, UserBadge.BirdProvider, UserBadge.FishProvider, UserBadge.RabbitProvider, UserBadge.RodentProvider, ReptileProvider, UserBadge.HorseProvider, UserBadge.LivestockProvider, UserBadge.OtherProvider
--from PetopiaUsers join CareProvider on PetopiaUsers.UserID = CareProvider.UserID
--				  join UserBadge on CareProvider.UserID = UserBadge.UserID;
--where PetopiaUsers.IsProvider = 1;

select PetopiaUsers.UserID as PetopiaUserID, PetopiaUsers.IsProvider, PetopiaUsers.FirstName, PetopiaUsers.LastName, 
		CareProvider.CareProviderID,
		DogProvider,CatProvider,BirdProvider,FishProvider,RabbitProvider,RodentProvider,ReptileProvider,HorseProvider,LivestockProvider,OtherProvider
from PetopiaUsers join CareProvider on PetopiaUsers.UserID = CareProvider.UserID
				  join UserBadge on CareProvider.UserID = UserBadge.UserID;

--SELECT t1.col, t3.col FROM table1 join table2 ON table1.primarykey = table2.foreignkey
--                                  join table3 ON table2.primarykey = table3.foreignkey
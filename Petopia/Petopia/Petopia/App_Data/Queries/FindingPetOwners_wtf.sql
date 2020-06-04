--select UserID, FirstName, LastName, ResZipCode 
--select PetopiaUsers.UserID, PetopiaUsers.IsProvider, PetopiaUsers.FirstName, PetopiaUsers.LastName, UserBadge.DogProvider, UserBadge.CatProvider, UserBadge.BirdProvider, UserBadge.FishProvider, UserBadge.RabbitProvider, UserBadge.RodentProvider, ReptileProvider, UserBadge.HorseProvider, UserBadge.LivestockProvider, UserBadge.OtherProvider
--from PetopiaUsers join PetOwner on PetopiaUsers.UserID = PetOwner.UserID
--				  join UserBadge on PetOwner.UserID = UserBadge.UserID;
--where PetopiaUsers.IsOwner = 1;

select PetopiaUsers.UserID as PetopiaUserID, PetopiaUsers.IsOwner, PetopiaUsers.FirstName, PetopiaUsers.LastName, PetopiaUsers.ResZipcode,
		PetOwner.PetOwnerID,
		DogOwner as dogO,CatOwner as catO,BirdOwner as birdO,FishOwner as fishO,RabbitOwner as rabbitO,RodentOwner as rodentO,
		ReptileProvider as reptileP,HorseProvider as horseP,LivestockProvider as livestockP,OtherProvider as otherP

from PetopiaUsers join PetOwner on PetopiaUsers.UserID = PetOwner.UserID
				  join UserBadge on PetOwner.UserID = UserBadge.UserID
				  --where PetopiaUsers.ResZipcode like '97%';

--SELECT t1.col, t3.col FROM table1 join table2 ON table1.primarykey = table2.foreignkey
--                                  join table3 ON table2.primarykey = table3.foreignkey
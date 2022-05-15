import {EntityRepository, Repository} from "typeorm";
import {FavouriteTracks} from "../Entities/FavouriteTracks";

@EntityRepository(FavouriteTracks)
export class FavouriteTracksRepository extends Repository<FavouriteTracks>{

}

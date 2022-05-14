import {EntityRepository, Repository} from "typeorm";
import {Playlist} from "../Entities/Playlist";

@EntityRepository(Playlist)
export class PlaylistsRepository extends Repository<Playlist>{

}

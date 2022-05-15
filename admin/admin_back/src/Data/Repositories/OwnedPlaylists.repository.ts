import {EntityRepository, Repository} from "typeorm";
import {OwnedPlaylists} from "../Entities/OwnedPlaylists";

@EntityRepository(OwnedPlaylists)
export class OwnedPlaylistsRepository extends Repository<OwnedPlaylists>{

}

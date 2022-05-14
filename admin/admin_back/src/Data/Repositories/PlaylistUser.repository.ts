import {EntityRepository, Repository} from "typeorm";
import {PlaylistUser} from "../Entities/PlaylistUser";

@EntityRepository(PlaylistUser)
export class PlaylistUserRepository extends Repository<PlaylistUser>{

}

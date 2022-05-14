import {EntityRepository, Repository} from 'typeorm'
import {Track} from "../Entities/Track";

@EntityRepository(Track)
export class TracksRepository extends Repository<Track>{
    async getFirst(){
        return await this.findOne();
    }
}

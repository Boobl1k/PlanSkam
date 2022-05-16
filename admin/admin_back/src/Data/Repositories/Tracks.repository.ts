import {EntityRepository, Repository, getRepository} from "typeorm";
import {Track} from "../Entities/Track";
import {Post} from "@nestjs/common";
import {TrackData} from "../Entities/TrackData";

@EntityRepository(Track)
export class TracksRepository extends Repository<Track> {
    @Post('removeTrack')
    async removeTrack(id: number) {
        const track = await this.findOne(id);
        if(track == null)
            return false;
        await this.remove(track);
        const trackDatasRepo = getRepository(TrackData);
        const trackData = await trackDatasRepo.findOne(track.TrackDataId);
        if(trackData != null)
            await trackDatasRepo.remove(trackData);
        return true;
    }
}

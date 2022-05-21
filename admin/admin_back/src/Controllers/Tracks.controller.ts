import {BadRequestException, Controller, Get, Post, Query} from "@nestjs/common";
import {TracksRepository} from "../Data/Repositories/Tracks.repository";

@Controller('tracks')
export class TracksController {
    constructor(private readonly tracksRepo: TracksRepository) {
    }

    @Post('removeTrack')
    async removeTrack(@Query("id") id: number) {
        if (await this.tracksRepo.removeTrack(id))
            return "Track has been removed";
        throw new BadRequestException();
    }

    @Get('searchTracks')
    async searchTracks(@Query('query') query: string) {
        return await this.tracksRepo.searchTracks(query);
    }

    @Post('changeName')
    async changeName(@Query('id') id: number, @Query('name') name: string) {
        if(await this.tracksRepo.changeName(id, name))
            return  "Name changed to " + name;
        throw new BadRequestException();
    }
}

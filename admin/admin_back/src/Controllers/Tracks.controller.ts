import {Controller, Get, Post, Query} from "@nestjs/common";
import {TracksRepository} from "../Data/Repositories/Tracks.repository";

@Controller('tracks')
export class TracksController {
    constructor(private readonly tracksRepo: TracksRepository) {
    }

    @Post('removeTrack')
    async removeTrack(@Query("id") id: number) {
        return await this.tracksRepo.removeTrack(id)
            ? "Track has been removed"
            : "Error";
    }

    @Get('searchTracks')
    async searchTracks(@Query('query') query: string) {
        return await this.tracksRepo.searchTracks(query);
    }
}

import {Controller, Get} from "@nestjs/common";
import {TracksRepository} from "../Data/Repositories/Tracks.repository";

@Controller('tracks')
export class TracksController{
    constructor(private readonly tracksRepo: TracksRepository) {
    }
    
    @Get('getfirst')
    async getFirst(): Promise<string>{
        return (await this.tracksRepo.getFirst()).Name;
    }
}
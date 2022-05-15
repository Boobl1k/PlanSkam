import {Module} from "@nestjs/common";
import {TypeOrmModule} from "@nestjs/typeorm";
import {Track} from "../Data/Entities/Track";
import {TracksRepository} from "../Data/Repositories/Tracks.repository";
import {TracksController} from "../Controllers/Tracks.controller";

@Module({
    imports: [TypeOrmModule.forFeature([Track, TracksRepository])],
    controllers: [TracksController]
})
export class TracksModule {
}
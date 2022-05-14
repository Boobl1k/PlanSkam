import {Module} from "@nestjs/common";
import {TypeOrmModule} from "@nestjs/typeorm";
import {Track} from "../Entities/Track";
import {TracksRepository} from "../Repositories/Tracks.repository";
import {TracksController} from "../Controllers/Tracks.controller";

@Module({
    imports: [TypeOrmModule.forFeature([Track, TracksRepository])],
    controllers: [TracksController]
})
export class TracksModule {
}
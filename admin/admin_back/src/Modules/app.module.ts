import {Module} from '@nestjs/common';
import {AppController} from '../Controllers/app.controller';
import {AppService} from '../Services/app.service';
import {TypeOrmModule} from '@nestjs/typeorm'
import {Track} from "../Data/Entities/Track";
import {UsersModule} from "./Users.module";
import {User} from "../Data/Entities/User";
import {Role} from "../Data/Entities/Role";
import {Playlist} from "../Data/Entities/Playlist";
import {PlaylistsModule} from "./Playlists.module";
import {Author} from "../Data/Entities/Author";

@Module({
    imports: [
        TypeOrmModule.forRoot({
            type: "mssql",
            host: "planscam.mssql.somee.com",
            database: "planscam",
            port: 1433,
            username: "erererererer123_SQLLogin_1",
            password: "2qnmximctf",
            entities: [Track, Role, User, Playlist, Author]
        }),
        UsersModule,
        PlaylistsModule
    ],
    controllers: [AppController],
    providers: [AppService],
})
export class AppModule {
}

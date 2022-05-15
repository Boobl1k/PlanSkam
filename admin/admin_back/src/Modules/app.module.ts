import {Module} from '@nestjs/common';
import {AppController} from '../Controllers/app.controller';
import {AppService} from '../Services/app.service';
import {TypeOrmModule} from '@nestjs/typeorm'
import {Track} from "../Data/Entities/Track";
import {TracksModule} from "./Tracks.Module";
import {UsersModule} from "./Users.module";
import {User} from "../Data/Entities/User";
import {Role} from "../Data/Entities/Role";

@Module({
    imports: [
        TypeOrmModule.forRoot({
            type: "mssql",
            host: "planscam.mssql.somee.com",
            database: "planscam",
            port: 1433,
            username: "erererererer123_SQLLogin_1",
            password: "2qnmximctf",
            entities: [Track, User, Role]
        }),
        TracksModule,
        UsersModule
    ],
    controllers: [AppController],
    providers: [AppService],
})
export class AppModule {
}

import {Module} from '@nestjs/common';
import {AppController} from '../Controllers/app.controller';
import {AppService} from '../Services/app.service';
import {TypeOrmModule} from '@nestjs/typeorm'
import {AspNetUser} from "../Entities/AspNetUser";

@Module({
    imports: [
        TypeOrmModule.forRoot({
            type: "mssql",
            host: "planscam.mssql.somee.com",
            database: "planscam",
            port: 1433,
            username: "erererererer123_SQLLogin_1",
            password: "2qnmximctf",
            entities: [AspNetUser]
        })
    ],
    controllers: [AppController],
    providers: [AppService],
})
export class AppModule {
}

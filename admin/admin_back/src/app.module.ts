import {Module} from '@nestjs/common';
import {AppController} from './Controllers/app.controller';
import {AppService} from './Services/app.service';
import {TypeOrmModule} from '@nestjs/typeorm'

@Module({
    imports: [
        TypeOrmModule.forRoot()
    ],
    controllers: [AppController],
    providers: [AppService],
})
export class AppModule {
}

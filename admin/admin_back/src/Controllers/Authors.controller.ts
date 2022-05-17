import {Controller, Get, Post, Query} from "@nestjs/common";
import {AuthorsRepository} from "../Data/Repositories/Authors.repository";

@Controller('authors')
export class AuthorsController {
    constructor(private readonly authorsRepo: AuthorsRepository) {
    }

    @Get('getWithTracks')
    async getWithTracks(@Query('authorId') authorId: number) {
        return this.authorsRepo.getWithTracks(authorId);
    }
}

import {EntityRepository, Repository} from "typeorm";
import {Author} from "../Entities/Author";

@EntityRepository(Author)
export class AuthorsRepository extends Repository<Author> {
    async getWithTracks(authorId: number) {
        return await this.findOne(authorId, {relations: ["Tracks"]});
    }
}

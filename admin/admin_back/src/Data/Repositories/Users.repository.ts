import {EntityRepository, Repository, getRepository} from "typeorm";
import {User} from "../Entities/User";
import {Role} from "../Entities/Role";

@EntityRepository(User)
export class UsersRepository extends Repository<User> {
    async GetAll() {
        return await this.find();
    }

    async IsAuthor(id: string) : Promise<boolean> {
        return (Boolean)((await this.createQueryBuilder('user')
            .where("user.Id = :id", {id})
            .innerJoin("user.Roles", "roles", "roles.Name = 'Author'")
            .select("case when count(*) > 0 then 1 else 0 end as isAuthor")
            .getRawOne<object>())["isAuthor"]);
    }

    async MakeAuthor(id: string) {
        const rolesRepo = getRepository(Role);
        const role = await rolesRepo
            .createQueryBuilder("role")
            .where("role.Name = 'Author'")
            .innerJoinAndSelect("role.Users", "users")
            .getOne();
        if (role == null)
            return false;
        const user = await this.findOne(id, {relations: ["Roles"]});
        if (user == null)
            return false;
        user.Roles.push(role);
        await this.save(user);
        return true;
    }

    async MakeNotAuthor(id: string) {
        const rolesRepo = getRepository(Role);
        const role = await rolesRepo
            .createQueryBuilder("role")
            .where("role.Name = 'Author'")
            .innerJoinAndSelect("role.Users", "users")
            .getOne();
        if (role == null)
            return false;
        const user = await this.findOne(id, {relations: ["Roles"]});
        if (user == null)
            return false;
        user.Roles.splice(user.Roles.indexOf(role), 1);
        await this.save(user);
        return true;
    }
}

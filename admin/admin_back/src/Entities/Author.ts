import {Entity, Column, PrimaryGeneratedColumn} from 'typeorm';

@Entity()
export class Author {
    @PrimaryGeneratedColumn()
    Id: number;

    @Column()
    Name: string;

    @Column()
    PictureId: number;

    @Column()
    UserId: number;
}
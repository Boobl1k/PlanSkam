import {Entity, Column, PrimaryGeneratedColumn, TableForeignKey} from 'typeorm';

@Entity()
export class Playlist {
    @PrimaryGeneratedColumn()
    Id: number;

    @Column()
    Name: string;

    @Column()
    OwnedById: number;
}
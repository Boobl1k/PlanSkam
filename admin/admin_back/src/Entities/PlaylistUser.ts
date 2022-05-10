import {Entity, Column, PrimaryGeneratedColumn} from 'typeorm';

//название таблицы не соответствует

@Entity()
export class PlaylistUser {
    @PrimaryGeneratedColumn()
    PlaylistId: number;

    @Column()
    UserId: string;
}